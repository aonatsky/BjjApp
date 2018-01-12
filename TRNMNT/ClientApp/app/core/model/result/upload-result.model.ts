import { UploadResultCode } from "../enum/upload-result-code.enum";

export interface IUploadResult {
    code: UploadResultCode;
    message: string;
}

export class UploadResult implements IUploadResult {
    public code: UploadResultCode;
    public message: string;
}

