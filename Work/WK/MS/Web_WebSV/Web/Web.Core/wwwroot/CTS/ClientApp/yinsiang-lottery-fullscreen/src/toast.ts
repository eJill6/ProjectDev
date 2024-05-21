const toast = (message: string) => {
    let messageDiv = document.createElement('div');
    messageDiv.className = 'toast_text';
    messageDiv.innerText = message;

    let div = document.createElement('div');
    div.className = 'toast';
    div.appendChild(messageDiv);

    document.body.appendChild(div);

    setTimeout(() => {
        div.parentNode?.removeChild(div);
    }, 1000);
};

export default toast;