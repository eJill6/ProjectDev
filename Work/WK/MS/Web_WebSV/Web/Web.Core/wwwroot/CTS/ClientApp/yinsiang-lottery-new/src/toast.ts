const toast = (message: string) => {
    let textDiv = document.createElement('div');
    textDiv.className = "toast_text";
    textDiv.innerText = message;

    let div = document.createElement('div');
    div.className = 'toast';
    div.appendChild(textDiv);

    document.body.appendChild(div);

    setTimeout(() => {
        div.parentNode?.removeChild(div);
    }, 1000);
};

export default toast;