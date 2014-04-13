using System;
using System.Collections.Generic;
using System.Text;

namespace CartographerLibrary
{
    /// <summary>
    /// Defines drawing tool
    /// </summary>
    public enum ToolType
    {
        None,
        Pointer,
        TableBlock,
        Beacon,
        Barrier,
        PolyLine,
        Text,
        Max
    };

    /// <summary>
    /// Context menu command types
    /// </summary>
    internal enum ContextMenuCommand
    {
        SelectAll,
        UnselectAll,
        Delete, 
        DeleteAll,
        MoveToFront,
        MoveToBack,
        Undo,
        Redo,
        SerProperties
    };
}
