import { authStorage } from "@/features/profile/auth-storage";
import { client } from "../gen/play-api/.kubb/client";

export const playApiUrl = process.env.EXPO_PUBLIC_API_URL;

if (!playApiUrl) {
  throw new Error("EXPO_PUBLIC_API_URL is required.");
}

client.setConfig({
  baseURL: playApiUrl,
  auth: async () => {
    const token = await authStorage.get();

    if (token === null) {
      return undefined;
    }

    return token;
  },
});
