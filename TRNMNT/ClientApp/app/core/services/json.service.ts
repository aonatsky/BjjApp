import {deserialize} from 'json-typescript-mapper';

export class JsonService {
    static deserialize<T>(Clazz: {new (): T;}, json: JSON): T {
        return deserialize(Clazz, json);
    }
}



