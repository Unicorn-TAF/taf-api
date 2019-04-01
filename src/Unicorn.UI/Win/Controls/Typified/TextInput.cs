﻿using System;
using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class TextInput : WinControl, ITextInput
    {
        public TextInput()
        {
        }

        public TextInput(IUIAutomationElement instance) : base(instance)
        {
        }

        public override int UiaType => UIA_ControlTypeIds.UIA_EditControlTypeId;

        public virtual string Value
        {
            get
            {
                if (Instance.CurrentClassName.Equals("PasswordBox"))
                {
                    return "The field is of PasswordBox type. Unable to get value";
                }
                else
                {
                    return this.ValuePattern.CurrentValue;
                }
            }
        }

        protected IUIAutomationValuePattern ValuePattern => 
            this.GetPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;

        public virtual void SendKeys(string text)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Send keys '{text}' to {this.ToString()}");

            var pattern = this.ValuePattern;

            if (pattern.CurrentIsReadOnly != 0)
            {
                throw new ControlInvalidStateException("Input is disabled");
            }

            pattern.SetValue(text);
        }

        public virtual bool SetValue(string text)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Set text '{text}' to {this.ToString()}");

            var pattern = this.ValuePattern;

            if (pattern.CurrentIsReadOnly != 0)
            {
                throw new ControlInvalidStateException("Input is disabled");
            }

            if (!this.Value.Equals(text, StringComparison.InvariantCultureIgnoreCase))
            {
                pattern.SetValue(text);
                return true;
            }
            else
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to set (input already has such text)");
                return false;
            }
        }
    }
}
