const toast = (messageObj:any) => {
    const message =
    messageObj instanceof Error ? (messageObj as Error).message : (messageObj as string);
    let contentDiv = document.createElement('div');
    contentDiv.className = "toast_text";
    contentDiv.innerText = message;

    let div = document.createElement('div');
    div.className = 'toast';
    div.appendChild(contentDiv);

    document.body.appendChild(div);

    setTimeout(() => {
        div.parentNode?.removeChild(div);
    }, 1000);
};

export default toast;