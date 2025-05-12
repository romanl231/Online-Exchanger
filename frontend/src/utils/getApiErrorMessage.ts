export const getApiErrorMessage = (err: any): string => {
  console.error("API Error:", err);

  const data = err.response?.data;

  if (typeof data === "string") return data; // <-- головне місце

  if (typeof data?.message === "string") return data.message;
  if (typeof data?.error === "string") return data.error;

  if (typeof data?.errors === "object") {
    const firstField = Object.values(data.errors)[0];
    if (Array.isArray(firstField) && typeof firstField[0] === "string") {
      return firstField[0];
    }
  }

  if (typeof err.message === "string") return err.message;

  return "Something went wrong";
};