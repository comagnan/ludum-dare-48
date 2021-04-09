using System;

namespace LD48.Dialogue
{
    public class DialogueEntry
    {
        public string Text { get; init; }
        public string Speaker { get; init; }
        public string Sprite { get; init; }
        public Action Callback { get; init; }
        public DialogueFont Font { get; init; }

        public DialogueEntry()
        {
            Font = DialogueFont.Standard;
        }
    }
}