export const getDeviceType = (): string => {
  const ua = navigator.userAgent;
  if (/mobile/i.test(ua)) return "Mobile";
  if (/tablet/i.test(ua)) return "Tablet";
  if (/iPad|Android|Touch/.test(ua)) return "Touch Device";
  return "Desktop";
};

export const getBrowserInfo = (): string => {
  const ua = navigator.userAgent;
  const browserMatch = ua.match(/(firefox|msie|chrome|safari|edg|opr|opera)[\/\s](\d+)/i);
  return browserMatch ? `${browserMatch[1]} ${browserMatch[2]}` : "Unknown Browser";
};

export const getDeviceFingerprint = (): string => {
  return `${getDeviceType()} - ${getBrowserInfo()}`;
};

export const getClientIp = async (): Promise<string> => {
  try {
    const res = await fetch("https://api64.ipify.org?format=json");
    const data = await res.json();
    return data.ip;
  } catch {
    return "Unknown IP";
  }
};
