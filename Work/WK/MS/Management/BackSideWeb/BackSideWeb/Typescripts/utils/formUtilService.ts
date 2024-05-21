class formUtilService {
    serializeObject($form: JQuery<HTMLElement>): any {
        let serializeString: string = $form.serialize();
        const formArray = serializeString.split("&");
        const formObject: { [key: string]: string } = {};

        for (let i = 0; i < formArray.length; i++) {
            if (formArray[i] == "") {
                continue;
            }

            const [key, value] = formArray[i].split("=");
            const decodedKey = decodeURIComponent(key);
            const decodedValue = decodeURIComponent(value);
            formObject[decodedKey] = decodedValue;
        }

        return formObject;
    }

    objectToFormData(obj): FormData {
        let formData = new FormData();

        for (let key in obj) {
            if (obj.hasOwnProperty(key)) {
                formData.append(key, obj[key]);
            }
        }

        return formData;
    }
}