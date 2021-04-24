using LD48.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48.Dialogue
{
    public interface IDialogueBox
    {
        void AddText(DialogueEntry p_Dialogue);
        bool IsActive();
        int GetBufferSize();
        void Update(GameTime p_GameTime,
                    in InputController p_InputController);
        void Draw(GameTime p_GameTime, SpriteBatch p_SpriteBatch);
    }
}