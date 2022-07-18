using Godot;
using System;

namespace HighscoreServiceClient.Scenes.ScoreScene
{
    public class ErrorDisplayManager : Node
    {
        [Export]
        public NodePath ServerErrorPopupPath;
        public Popup _serverError;

        [Export]
        public NodePath InvalidNicknamePath;
        public Popup _invalidNickname;

        public override void _Ready()
        {
            _serverError = GetNode<Popup>(ServerErrorPopupPath);
            _invalidNickname = GetNode<Popup>(InvalidNicknamePath);
        }

        public void DisplayErrorPopup(ErrorPopup errorPopup)
        {
            switch (errorPopup)
            {
                case ErrorPopup.ServerError:
                    _serverError.PopupCenteredClamped();
                    break;
                case ErrorPopup.InvalidNickname:
                    _invalidNickname.PopupCenteredClamped();
                    break;
                default:
                    throw new ArgumentException($"Invalid popup provided: {errorPopup}", nameof(errorPopup));
            }
        }
    }
}