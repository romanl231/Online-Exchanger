import { getClientIp, getDeviceType  } from "../utils/getClientInfo";

export const ClientInfoService = {
    getClientData: async () => {
        const ipAddress = await getClientIp();
        const deviceType = getDeviceType();
        return {ipAddress, deviceType};
    }    
}