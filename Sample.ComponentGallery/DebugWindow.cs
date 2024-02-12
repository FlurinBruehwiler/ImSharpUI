using Flamui;
using Flamui.UiElements;

namespace Sample.ComponentGallery;

public class DebugWindow(EventLoop eventLoop) : FlamuiComponent
{
    private UiElement? _selectedUiElement;

    public override void Build(Ui ui)
    {
        var otherWindow = eventLoop.Windows.First(x => x != ui.Window);

        ui.DivStart().Dir(Dir.Horizontal).Padding(10).Gap(10).Color(C.Background);
            ui.DivStart().Gap(5);
                DisplayUiElement(ui, otherWindow.RootContainer,  39210, 1);
            ui.DivEnd();

            ui.DivStart();
                if (_selectedUiElement is not null)
                {
                    DisplayDetail(ui, _selectedUiElement);
                }
            ui.DivEnd();

        ui.DivEnd();
    }

    private void DisplayUiElement(Ui ui, UiElement uiElement, int parentHash, int indentationLevel)
    {
        var key = Ui.S(uiElement.Id.GetHashCode() + parentHash);

        ui.DivStart(out var div, key).PaddingLeft(indentationLevel * 10).Height(20).Border(1, C.Border).Rounded(2);
            ui.Text(ToString(uiElement));
        ui.DivEnd();

        if (div.IsClicked)
        {
            _selectedUiElement = uiElement;
        }

        if (_selectedUiElement == uiElement)
        {
            div.Color(C.Selected);
        }
        else
        {
            div.Color(C.Transparent);
        }

        if (uiElement is UiElementContainer container)
        {
            foreach (var containerChild in container.Children)
            {
                DisplayUiElement(ui, containerChild, uiElement.Id.GetHashCode(), indentationLevel + 1);
            }
        }
    }

    private void DisplayDetail(Ui ui, UiElement uiElement)
    {
        foreach (var propertyInfo in uiElement.GetType().GetProperties())
        {
            ui.DivStart(propertyInfo.Name).Height(20);
                ui.Text($"{propertyInfo.Name}: {propertyInfo.GetValue(uiElement)}");
            ui.DivEnd();
        }
    }

    private string ToString(UiElement uiElement)
    {
        if (uiElement is UiContainer)
        {
            return nameof(UiContainer);
        }

        if (uiElement is UiText uiText)
        {
            return Ui.S(uiText.Content, x => $"{nameof(uiText)}: {x}");
        }

        if (uiElement is UiImage uiImage)
        {
            return Ui.S(uiImage.Src, x => $"{nameof(uiText)}: {x}");
        }

        if (uiElement is UiSvg uiSvg)
        {
            return Ui.S(uiSvg.Src, x => $"{nameof(uiText)}: {x}");
        }

        return uiElement.ToString() ?? string.Empty;
    }
}
