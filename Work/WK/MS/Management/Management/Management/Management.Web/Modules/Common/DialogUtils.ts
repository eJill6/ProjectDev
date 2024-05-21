
    export function openGridDialog(dialog: any, asPanel?: boolean) {
        var panel = (asPanel != undefined) ? asPanel : false;
        dialog.dialogOpen(panel);
    }

