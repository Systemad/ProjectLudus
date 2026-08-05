import { client } from "@/gen/.kubb/client";

const baseURL = process.env.EXPO_PUBLIC_API_URL;

if (!baseURL) {
  throw new Error("EXPO_PUBLIC_API_URL is required.");
}

client.setConfig({ baseURL });
