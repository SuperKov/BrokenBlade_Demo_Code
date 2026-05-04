using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Markup;
using Yarn.Unity;

public class DialogueMenu : DialoguePresenterBase
{
    [SerializeField] private CanvasGroup Group;
    [SerializeField] private float FadeUpDuration;
    [SerializeField] private Button ContinueButton;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI LineText;
    [SerializeField] private TextMeshProUGUI NameText;

    [SerializeField] List<ActionMarkupHandler> eventHandlers = new();
    private List<IActionMarkupHandler> ActionMarkupHandlers
    {
        get
        {
            var pauser = new PauseEventProcessor();
            List<IActionMarkupHandler> ActionMarkupHandlers = new()
                {
                    pauser,
                };
            ActionMarkupHandlers.AddRange(eventHandlers);
            return ActionMarkupHandlers;
        }
    }

    public override YarnTask OnDialogueCompleteAsync()
    {
        if (Group != null)
        {
            Group.alpha = 0;
        }
        return YarnTask.CompletedTask;
    }

    public override YarnTask OnDialogueStartedAsync()
    {
        if (Group != null)
        {
            Group.alpha = 0;
        }
        return YarnTask.CompletedTask;
    }

    public override async YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token)
    {
        if (LineText == null)
        {
            Debug.LogError($"{nameof(LinePresenter)} does not have a text view. Skipping line {line.TextID} (\"{line.RawText}\")");
            return;
        }

        MarkupParseResult text;

        text = line.TextWithoutCharacterName;

        Typewriter ??= new InstantTypewriter()
        {
            ActionMarkupHandlers = ActionMarkupHandlers,
            Text = LineText,
        };

        Typewriter.PrepareForContent(text);

        if (Group != null)
            await Effects.FadeAlphaAsync(Group, 0, 1, FadeUpDuration, token.HurryUpToken);

        await Typewriter.RunTypewriter(text, token.HurryUpToken).SuppressCancellationThrow();

        await YarnTask.WaitUntilCanceled(token.NextContentToken).SuppressCancellationThrow();

        Typewriter.ContentWillDismiss();

        await Effects.FadeAlphaAsync(Group, 1, 0, FadeUpDuration, token.HurryUpToken);
    }
}