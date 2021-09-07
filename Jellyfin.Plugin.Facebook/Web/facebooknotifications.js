const FacebookConfigurationPageVar = {
    pluginId: '0df52034-4b59-443f-ac40-1d3f5107e2da'
};

function loadUserConfig(page, userId) {
    Dashboard.showLoadingMsg();
    ApiClient.getPluginConfiguration(FacebookConfigurationPageVar.pluginId).then(function (config) {
        const facebookConfig = config.Options.filter(function (c) {
            return userId === c.JellyfinUserId;
        })[0] || {};

        page.querySelector('#chkEnableFacebook').checked = facebookConfig.IsEnabled || false;
        page.querySelector('#txtFacebookWebhookUrl').value = facebookConfig.WebHookUrl || '';
        page.querySelector('#txtFacebookWebhookUsername').value = facebookConfig.Username || '';
        page.querySelector('#txtFacebookWebhookIconUrl').value = facebookConfig.IconUrl || '';
    });

    Dashboard.hideLoadingMsg();
}

export default function (view) {

    view.querySelector('#selectUser').addEventListener('change', function () {
        loadUserConfig(view, this.value);
    });

    view.querySelector('#testNotification').addEventListener('click', function () {
        Dashboard.showLoadingMsg();
        const onError = function () {
            Dashboard.alert('There was an error sending the test notification. Please check your notification settings and try again.');
            Dashboard.hideLoadingMsg();
        };

        ApiClient.getPluginConfiguration(FacebookConfigurationPageVar.pluginId).then(function (config) {
            config.Options.map(function (c) {
                if (c.WebHookUrl === '') {
                    Dashboard.hideLoadingMsg();
                    Dashboard.alert('Please configure and save at least one notification account.');
                }

                ApiClient.ajax({
                    type: 'POST',
                    url: ApiClient.getUrl('Notification/Facebook/Test/' + c.JellyfinUserId)
                }).then(function () {
                    Dashboard.hideLoadingMsg();
                }, onError);
            });
        });
    });

    view.querySelector('.FacebookConfigurationForm').addEventListener('submit', function (e) {
        Dashboard.showLoadingMsg();
        const form = this;

        ApiClient.getPluginConfiguration(FacebookConfigurationPageVar.pluginId).then(function (config) {
            const userId = form.querySelector('#selectUser').value;
            let facebookConfig = config.Options.filter(function (c) {
                return userId === c.JellyfinUserId;
            })[0];

            if (!facebookConfig) {
                facebookConfig = {};
                config.Options.push(facebookConfig);
            }

            facebookConfig.JellyfinUserId = userId;
            facebookConfig.IsEnabled = form.querySelector('#chkEnableFacebook').checked;
            facebookConfig.WebHookUrl = form.querySelector('#txtFacebookWebhookUrl').value;
            facebookConfig.Username = form.querySelector('#txtFacebookWebhookUsername').value;
            facebookConfig.IconUrl = form.querySelector('#txtFacebookWebhookIconUrl').value;

            ApiClient.updatePluginConfiguration(FacebookConfigurationPageVar.pluginId, config).then(function (result) {
                Dashboard.processPluginConfigurationUpdateResult(result);
            });
        });
        e.preventDefault();
        return false;
    });

    view.addEventListener('viewshow', function () {
        Dashboard.showLoadingMsg();
        const page = this;

        ApiClient.getUsers().then(function (users) {
            const selUser = page.querySelector('#selectUser');
            selUser.innerHTML = users.map(function (user) {
                return '<option value="' + user.Id + '">' + user.Name + '</option>';
            });
            selUser.dispatchEvent(new Event('change', {
                bubbles: true,
                cancelable: false
            }));
        });

        Dashboard.hideLoadingMsg();
    });
}
