import messageOpenUrl from '@/message';

const promptDialog = (message: string, url:string,logonMode:number) => {

    let defaultBtnDiv = document.createElement('div');
    defaultBtnDiv.className = 'btn default';
    defaultBtnDiv.innerText = '取消';
    
    let highlightBtnDiv = document.createElement('div');
    highlightBtnDiv.className = 'btn highlight';
    highlightBtnDiv.innerText = '去充值';
    
    let h1 = document.createElement('h1');
    h1.innerText = `提示`;

    let p = document.createElement('p');
    p.innerText = message;

    let textDiv = document.createElement('div');
    textDiv.className = 'popup_text';
    textDiv.appendChild(h1);
    textDiv.appendChild(p);

    let mainDiv = document.createElement('div');
    mainDiv.className = 'modal_main';
    mainDiv.appendChild(textDiv);

    let btnDiv = document.createElement('div');
    btnDiv.className = 'modal_btn';
    btnDiv.appendChild(defaultBtnDiv);
    btnDiv.appendChild(highlightBtnDiv);

    let contentDiv = document.createElement('div');
    contentDiv.className = 'content_frame';
    contentDiv.appendChild(mainDiv);
    contentDiv.appendChild(btnDiv);

    let frameDiv = document.createElement('div');
    frameDiv.className = 'flex_frame';
    frameDiv.appendChild(contentDiv);

    let promptDiv = document.createElement('div');
    promptDiv.className = 'modal prompt';
    promptDiv.appendChild(frameDiv);    

    let popupCoverDiv = document.createElement('div');
    popupCoverDiv.className = 'popup-cover';
    popupCoverDiv.style.cssText = 'position:absolute;right:0;bottom:0;top:0;left:0;';
    popupCoverDiv.appendChild(promptDiv);
    document.body.appendChild(popupCoverDiv);

    popupCoverDiv.onclick = () =>{
        popupCoverDiv.parentNode?.removeChild(popupCoverDiv);
    }
    
    defaultBtnDiv.onclick = () => {           
        popupCoverDiv.parentNode?.removeChild(popupCoverDiv);
    }

    highlightBtnDiv.onclick = () => {
        // location.href = url;
        // popupCoverDiv.parentNode?.removeChild(popupCoverDiv);
        messageOpenUrl.openUrl(logonMode,url,null);
    }
};


export default promptDialog;