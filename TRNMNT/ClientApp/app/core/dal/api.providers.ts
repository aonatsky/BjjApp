import { DataApiService } from './api/data-api.service'
import { DataService } from './contracts/data.service'

export const ApiProviders = [
    {
        provide: DataService,
        useClass: DataApiService
    }
    ]