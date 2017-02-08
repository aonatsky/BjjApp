import {Injectable} from '@angular/core';
import {Http} from '@angular/http';
import {BaseServer} from './base.server';
import {ServerSettingsService} from '../server.settings.service';
import {LoggerService} from '../../../core/services/logger.service';

@Injectable()
export class ApiServer extends BaseServer {
    constructor(serverSettings: ServerSettingsService, loggerService: LoggerService,  http: Http ) {
        super(serverSettings.getApiEndpoint(), loggerService, http)
    }
}
