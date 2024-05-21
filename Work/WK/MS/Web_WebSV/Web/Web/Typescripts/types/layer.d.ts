declare var layer: {
    alert(content: string, options: any, yse?: Function),
    confirm(content: string, options: any, yse: Function, cancel: Function),
    close(index: number),
    open(options: any),
    full(index?: number),
    msg(message: string, options: any),
    index: number
}