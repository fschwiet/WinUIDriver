using System;
using System.Reflection;
using System.Threading;
using System.Windows.Automation;

namespace WinUIDriver
{
    public class WinUIDriver
    {
        public static AutomationElement SearchUIElements(AutomationElement root, TreeScope depth, PropertyCondition filter)
        {
            var waitInterval = 50;
            var maxWait = 5 * 1000;

            if (root == null)
                return null;

            var startTime = DateTime.UtcNow;

            AutomationElementCollection uiElements = root.FindAll(depth, filter);

            while (uiElements.Count == 0 && (DateTime.UtcNow - startTime).TotalMilliseconds <= maxWait)
            {
                Thread.Sleep(waitInterval);

                uiElements = root.FindAll(depth, filter);
            }

            if (uiElements.Count > 1)
                throw new MissingUIElementException("Expected to find unique ui element, found " + uiElements.Count);
            else if (uiElements.Count == 0)
                throw new MissingUIElementException("Expected to find unique ui element, did not find any before timeout.");

            return uiElements[0];
        }

        public static AutomationElement SearchUIElementsByAccessibilityName(AutomationElement uiElement, TreeScope searchDepth, string accessibilityName)
        {
            return SearchUIElements(uiElement, searchDepth,
                new PropertyCondition(
                    AutomationElement.NameProperty,
                    accessibilityName));
        }

        public static TReturnValue UsePattern<TPattern, TReturnValue>(AutomationElement uiElement, Func<TPattern, TReturnValue> action) where TPattern : BasePattern
        {
            var patternType = typeof(TPattern);

            AutomationPattern patternInstance = null;
            var patternInstanceField = patternType.GetField("Pattern", BindingFlags.Static | BindingFlags.Public);

            if (patternInstanceField != null)
                patternInstance = patternInstanceField.GetValue(null) as AutomationPattern;

            if (patternInstance == null)
            {
                var patternInstanceProperty = patternType.GetProperty("Pattern", BindingFlags.Static | BindingFlags.Public);

                if (patternInstanceProperty == null)
                    throw new Exception("Could not find AutomationPattern instance for " + patternType.Name + " -- simple heuristic failure.");

                patternInstance = patternInstanceProperty.GetValue(null, null) as AutomationPattern;
            }

            var patternForElement = uiElement.GetCurrentPattern(patternInstance) as TPattern;

            return action(patternForElement);
        }

        public static void UsePattern<TPattern>(AutomationElement uiElement, Action<TPattern> action) where TPattern : BasePattern
        {
            UsePattern<TPattern, object>(uiElement, o =>
            {
                action(o);
                return null;
            });
        }

        public static void InvokeUIElement(AutomationElement uiElement)
        {
            UsePattern<InvokePattern>(uiElement, p => p.Invoke());
        }

        public static string GetUIElementText(AutomationElement uiElement)
        {
            return UsePattern<TextPattern, String>(uiElement, p => p.DocumentRange.GetText(int.MaxValue));
        }

        public static string GetUIElementValue(AutomationElement uiElement)
        {
            return UsePattern<ValuePattern, string>(uiElement, p => p.Current.Value);
        }

        public static void SetUIElementValue(AutomationElement uiElement, string value)
        {
            UsePattern<ValuePattern>(uiElement, p => p.SetValue(value));
        }
    }
}
