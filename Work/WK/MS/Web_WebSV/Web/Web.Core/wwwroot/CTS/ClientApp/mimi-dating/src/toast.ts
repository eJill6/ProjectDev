const toast = (messageObj:any) => {
    
    const message =
    messageObj instanceof Error ? (messageObj as Error).message : (messageObj as string);
    // let contentDiv = document.createElement('div');
    // contentDiv.className = "toast_text";
    // contentDiv.innerText = message;

    // let div = document.createElement('div');
    // div.className = 'toast';
    // div.appendChild(contentDiv);

    // document.body.appendChild(div);

    // setTimeout(() => {
    //     div.parentNode?.removeChild(div);
    // }, 1000);


    let toastDiv=document.createElement("div");
    toastDiv.className="toast";
    let toast_outerDiv=document.createElement("div");
    toast_outerDiv.className="toast_outer"
    let toast_textDiv=document.createElement("div");
    toast_textDiv.className="toast_text";
    toast_textDiv.innerText=message;

    toast_outerDiv.appendChild(toast_textDiv);
    toastDiv.appendChild(toast_outerDiv);
    document.body.appendChild(toastDiv)
    
    setTimeout(() => {
        toastDiv.parentNode?.removeChild(toastDiv);
    }, 1000);



};

export default toast;