import {ApiServer} from './servers/api.server';
import { DataApiService } from './api/data-api.service'
import { DataFakeService } from './fake/data-fake.service'
import { DataService } from './contracts/data.service'

export const ApiProviders = [
    {
        provide: DataService,
        useClass: DataFakeService
    },
    {
        provide: ApiServer,
        useClass: ApiServer
    }    
    ]