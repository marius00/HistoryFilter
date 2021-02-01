# HistoryFilter

A simple tool to filter out entries from "recent files" and "quick access" in windows.  
Useful for removing quick access links to files on pendrives and similar.

The program starts minimized to tray (look for a book icon), double click the icon to open the UI.  
In the UI you can add prefixes which will be removed from "recent files" and "quick access".


## Example:
`Z:\` will remove all history from the `Z:` drive.  
`M:\Documents\` will remove all history from the `Documents` folder on the `M:` drive.  

The entries are scanned and removed at 1 minute intervals.  
If quick access is open while the scan is being run, it may not be successfully removed.  