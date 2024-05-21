const toast = (message: string) => {
    let p = document.createElement('p');
    p.innerText = message;

    let div = document.createElement('div');
    div.className = 'toast position-fixed top-50 start-50 translate-middle fs-3 text-center p-4 text-white text-break';
    div.appendChild(p);

    document.body.appendChild(div);

    setTimeout(() => {
        div.parentNode?.removeChild(div);
    }, 1000);
};

export default toast;