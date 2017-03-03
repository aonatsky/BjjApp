import {Injectable} from '@angular/core';
import {AppConfig} from '../../app.config';

@Injectable()
export class ServerSettingsService {
    getApiEndpoint(): string {
        return AppConfig.apiEndpoint;
    }
}
