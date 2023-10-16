﻿using System.Collections.ObjectModel;
using ImSharpUISample.UiElements;
using static ImSharpUISample.Ui;

namespace ImSharpUISample;

public interface IData
{
    public UiElementId Id { get; set; }
}

[AttributeUsage(AttributeTargets.Method)]
public class BuilderAttribute : Attribute
{

}

public class ModalComponent
{
    private bool _wasShown;

    [Builder]
    public void StartModal()
    {

    }

    [Builder]
    public void EndModal(ref bool show, string title, List<UiElement> children)
    {
        DivStart().Absolute(Root).XAlign(XAlign.Center).MAlign(MAlign.Center).ZIndex(1).Hidden(!show);
            DivStart(out var modalDiv).Clip().Color(39, 41, 44).Width(400).Height(200).Radius(10).BorderWidth(2).BorderColor(58, 62, 67);

                if (_wasShown && TryGetMouseClickPosition(out var pos) && !modalDiv.ContainsPoint(pos.X, pos.Y))
                    show = false;

                _wasShown = show;

                //Header
                DivStart().Height(25).Dir(Dir.Horizontal).MAlign(MAlign.SpaceBetween).PaddingLeft(10);
                    //Title
                    DivStart();
                        Text(title).VAlign(TextAlign.Center);
                    DivEnd();

                    //Close button
                    DivStart(out var closeButton).Width(25);
                        if (closeButton.Clicked)
                            show = false;
                        SvgImage("close.svg");
                    DivEnd();
                DivEnd();

                //Main Content
                DivStart().Padding(15);

                    //start user content
                    DivStart(out var userContentWrapper);

                    ((UiContainer)userContentWrapper).Children = children.ToList();

                    //End user content
                    DivEnd();

                    //Footer
                    DivStart().Height(25).Dir(Dir.Horizontal).MAlign(MAlign.FlexEnd).Gap(10);
                        DivStart().Width(70).Color(53, 116, 240).Radius(3);
                            Text("Next").VAlign(TextAlign.Center).HAlign(TextAlign.Center).Color(230, 230, 230);
                        DivEnd();
                        DivStart(out var cancelButton).Width(70).Color(43, 45, 48).Radius(3).BorderWidth(1).BorderColor(100, 100, 100);
                            if (cancelButton.Clicked)
                                show = false;

                            Text("Cancel").VAlign(TextAlign.Center).HAlign(TextAlign.Center).Color(230, 230, 230);;
                        DivEnd();
                    DivEnd();

                DivEnd();
            DivEnd();
        DivEnd();
    }
}

public class DropDownComponent
{
    [Builder]
    public static void StartDropDown()
    {

    }

    [Builder]
    public static void DropDownEntry()
    {

    }

    [Builder]
    public static void EndDropDown()
    {

    }
}
