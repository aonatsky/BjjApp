import { AuthServiceConfig, FacebookLoginProvider } from 'angular5-social-login'; 

export function getAuthServiceConfigs() {
    const config = new AuthServiceConfig([
        {
            id: FacebookLoginProvider.PROVIDER_ID,
            provider: new FacebookLoginProvider('1400305110017831')
        }
    ]);
    return config;
}