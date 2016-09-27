using System;

namespace MindMate.View.NoteEditing
{

    /// <summary>
    /// Struct used to define a Html Table
    /// Html Defaults are based on FrontPage default table
    /// </summary>
    [Serializable]
    public struct HtmlTableProperty
    {
        // properties defined for the table
        public string CaptionText;
        public HorizontalAlignOption CaptionAlignment;
        public VerticalAlignOption CaptionLocation;
        public byte BorderSize;
        public HorizontalAlignOption TableAlignment;
        public byte TableRows;
        public byte TableColumns;
        public ushort TableWidth;
        public MeasurementOption TableWidthMeasurement;
        public byte CellPadding;
        public byte CellSpacing;


        // constructor defining a base table with default attributes
        public HtmlTableProperty(bool htmlDefaults)
        {
            //define base values
            CaptionText = string.Empty;
            CaptionAlignment = HorizontalAlignOption.Default;
            CaptionLocation = VerticalAlignOption.Bottom;
            TableAlignment = HorizontalAlignOption.Center;

            // define values based on whether HTML defaults are required
            if (htmlDefaults)
            {
                BorderSize = 1;
                TableRows = 3;
                TableColumns = 3;
                TableWidth = 90;
                TableWidthMeasurement = MeasurementOption.Percent;
                CellPadding = 1;
                CellSpacing = 0;
            }
            else
            {
                BorderSize = 0;
                TableRows = 1;
                TableColumns = 1;
                TableWidth = 0;
                TableWidthMeasurement = MeasurementOption.Pixel;
                CellPadding = 0;
                CellSpacing = 0;
            }
        }

    } //HtmlTableProperty

    // Enum used to define the text alignment property
    public enum HorizontalAlignOption
    {
        Default,
        Left,
        Center,
        Right

    } //HorizontalAlignOption


    // Enum used to define the vertical alignment property
    public enum VerticalAlignOption
    {
        Default,
        Top,
        Bottom

    } //VerticalAlignOption

    // Enum used to define the unit of measure for a value
    public enum MeasurementOption
    {
        Pixel,
        Percent

    } //MeasurementOption

}