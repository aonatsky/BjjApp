﻿import { UploadResultCode } from '../enum/upload-result-code.enum';

export interface IUploadResult {
    code: UploadResultCode;
    messages: string[];
}

export class UploadResult implements IUploadResult {
    code: UploadResultCode;
    messages: string[];
}

