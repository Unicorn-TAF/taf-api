﻿namespace Unicorn.UI.Core.Controls.Interfaces.Typified
{
    public interface ITextInput
    {
        string Value
        {
            get;
        }

        void SendKeys(string text);

        bool SetText(string text);
    }
}