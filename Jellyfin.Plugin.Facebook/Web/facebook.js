const pluginId = '0df52034-4b59-443f-ac40-1d3f5107e2da';

export default function (view) {
    
    ApiClient.getPluginConfiguration(pluginId)
    .then(config => {

        view.querySelector('#enable').checked = config.IsEnabled || false;
        view.querySelector('#groupId').value = config.GroupId || '';
        view.querySelector('#accessToken').value = config.AccessToken || '';

        return config;
    })
    .then(config => {

        view.querySelector('#test').addEventListener('click', () => {
            Dashboard.showLoadingMsg();
        
            ApiClient.ajax({
                type: 'POST',
                url: ApiClient.getUrl('Notification/Facebook/Test')
            }).then(() => {
                Dashboard.hideLoadingMsg();
            }, () => {
                Dashboard.alert('Error sending test notification.');
                Dashboard.hideLoadingMsg();
            });
        });
    
        view.querySelector('#save').addEventListener('click', () => {

            Dashboard.showLoadingMsg();
    
            config.JellyfinUserId = 0;
            config.IsEnabled = view.querySelector('#enable').checked;
            config.GroupId = view.querySelector('#groupId').value;
            config.AccessToken = view.querySelector('#accessToken').value;
    
            ApiClient.updatePluginConfiguration(pluginId, config).then(result => {

                Dashboard.processPluginConfigurationUpdateResult(result);
    
                Dashboard.alert('Settings saved.');
            });
        });
    });
}
