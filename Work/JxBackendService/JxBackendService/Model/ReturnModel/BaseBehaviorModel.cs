using JxBackendService.Model.Param.Client;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ReturnModel
{
    public class BaseBehaviorModel
    {
        public ClientBehavior ClientBehavior { get; set; }
    }

    public static class BehaviorModelExtensions
    {
        public static void SetBehaviorRedirectPage(this BaseBehaviorModel behaviorModel, ClientAppPage clientAppPage)
        {
            behaviorModel.ClientBehavior = new ClientBehavior()
            {
                ClientActionType = ClientActionType.Redirect.Value,
                RedirectPage = clientAppPage.Value
            };
        }

        public static void SetBehaviorToast(this BaseBehaviorModel behaviorModel, string message)
        {
            behaviorModel.ClientBehavior = new ClientBehavior()
            {
                ClientActionType = ClientActionType.Toast.Value,
                DialogSetting = new DialogSetting() { Message = message },
            };
        }

        public static void SetBehaviorErrorDialog(this BaseBehaviorModel behaviorModel, string message)
        {
            behaviorModel.SetBehaviorAlertDialog(CommonElement.Error, message, null);
        }

        public static void SetBehaviorErrorDialog(this BaseBehaviorModel behaviorModel, string message, ClientAppPage redirectPageAfterConfirm)
        {
            behaviorModel.SetBehaviorAlertDialog(CommonElement.Error, message, redirectPageAfterConfirm);
        }

        public static void SetBehaviorAlertDialog(this BaseBehaviorModel behaviorModel, string title, string message)
        {
            behaviorModel.SetBehaviorAlertDialog(title, message, null);
        }

        public static void SetBehaviorAlertDialog(this BaseBehaviorModel behaviorModel, string title, string message, ClientAppPage redirectPageAfterConfirm)
        {
            var dialogModel = new DialogSetting()
            {
                Title = title,
                Message = message
            };

            if (redirectPageAfterConfirm != null)
            {
                dialogModel.RedirectPageAfterConfirm = redirectPageAfterConfirm.Value;
            }

            behaviorModel.ClientBehavior = new ClientBehavior()
            {
                ClientActionType = ClientActionType.AlertDialog.Value,
                DialogSetting = dialogModel
            };
        }

        public static void SetBehaviorConfirmDialog(this BaseBehaviorModel behaviorModel,
            string title, string message, ClientAppPage redirectPageAfterConfirm, ClientAppPage redirectPageAfterCancel)
        {
            behaviorModel.SetBehaviorConfirmDialog(
                title,
                message,
                CommonElement.Confirm,
                CommonElement.Cancel,
                redirectPageAfterConfirm,
                redirectPageAfterCancel);
        }

        public static void SetBehaviorConfirmDialog(this BaseBehaviorModel behaviorModel,
            string title, string message, string confirmText, string cancelText,
            ClientAppPage redirectPageAfterConfirm, ClientAppPage redirectPageAfterCancel)
        {
            behaviorModel.BaseSetBehaviorConfirmDialog(
                ClientActionType.ConfirmDialog,
                title,
                message,
                confirmText,
                cancelText,
                redirectPageAfterConfirm,
                redirectPageAfterCancel);
        }

        public static void SetBehaviorGoogleAuthenticatorConfirmDialog(this BaseBehaviorModel behaviorModel,
            string title, string message, string confirmText, string cancelText,
            ClientAppPage redirectPageAfterConfirm, ClientAppPage redirectPageAfterCancel)
        {
            behaviorModel.BaseSetBehaviorConfirmDialog(
                ClientActionType.ConfirmDialog,
                title,
                message,
                confirmText,
                cancelText,
                redirectPageAfterConfirm,
                redirectPageAfterCancel);
        }

        public static void SetBehavior(this BaseBehaviorModel behaviorModel, BaseReturnModel returnModel, string title, ClientAppPage redirectPageAfterConfirm)
        {
            if (returnModel.IsSuccess)
            {
                behaviorModel.SetBehaviorAlertDialog(title, returnModel.Message, redirectPageAfterConfirm);
            }
            else
            {
                behaviorModel.SetBehaviorErrorDialog(returnModel.Message);
            }
        }

        private static void BaseSetBehaviorConfirmDialog(this BaseBehaviorModel behaviorModel, ClientActionType clientActionType,
            string title, string message, string confirmText, string cancelText,
            ClientAppPage redirectPageAfterConfirm, ClientAppPage redirectPageAfterCancel)
        {
            if (clientActionType != ClientActionType.ConfirmDialog)
            {
                throw new ArgumentOutOfRangeException();
            }

            var confirmDialogModel = new DialogSetting()
            {
                Title = title,
                Message = message,
                ConfirmText = confirmText,
                CancelText = cancelText,
            };

            if (redirectPageAfterConfirm != null)
            {
                confirmDialogModel.RedirectPageAfterConfirm = redirectPageAfterConfirm.Value;
            }

            if (redirectPageAfterCancel != null)
            {
                confirmDialogModel.RedirectPageAfterCancel = redirectPageAfterCancel.Value;
            }

            behaviorModel.ClientBehavior = new ClientBehavior()
            {
                ClientActionType = ClientActionType.ConfirmDialog.Value,
                DialogSetting = confirmDialogModel
            };
        }

    }
}
