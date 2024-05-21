enum BackSideWebUserActionTypes {
    logout = 1,
    changePassword = 2,
}

interface IBackSideWebUserMessage {
    BackSideWebUserActionType: BackSideWebUserActionTypes,
    Message: string
}

class bwUserMessageService {
    private messageQueue: MessageQueue;

    connectMessageQueue(userId: string) {
        this.messageQueue = new MessageQueue(userId);        
    }

    getRoutingKey(): string {
        return this.messageQueue.getRoutingKey();
    }

    processMessage(backSideWebUserMessage: IBackSideWebUserMessage) {
        let service = this.getService(backSideWebUserMessage.BackSideWebUserActionType);

        if (backSideWebUserMessage.Message) {
            $.alert(backSideWebUserMessage.Message, "确定", service.doJob);

            return;
        }

        service.doJob();
    }

    private getService(managementActionType: BackSideWebUserActionTypes): IProcessMessageService {
        switch (managementActionType) {
            case BackSideWebUserActionTypes.logout: {
                return new processLogoutMessageService();
            }
            case BackSideWebUserActionTypes.changePassword: {
                return new processChangePasswordMessageService();
            }
        }
    }
}

interface IProcessMessageService {
    doJob();
}

class processLogoutMessageService implements IProcessMessageService {
    doJob() {
        location.href = globalVariables.GetUrl("Authority/Logout");
    }
}

class processChangePasswordMessageService implements IProcessMessageService {
    doJob() {
        location.href = globalVariables.GetUrl("ChangePassword");
    }
}