import { z } from "zod";

export const authTokenSchema = z.string().min(1);

export const authCodeSchema = z.string().min(1);

export const userResponseSchema = z.object({
  id: z.string().min(1),
  steamId: z.string().min(1),
  steamName: z.string().min(1),
  avatarUrl: z.string().url(),
  role: z.string().min(1),
});

const listSummaryResponseSchema = z.object({
  id: z.string().min(1),
  name: z.string(),
  description: z.null(),
  visibility: z.enum(["Private", "Public"]),
  isDefault: z.boolean(),
  itemCount: z.number().int().nonnegative(),
  createdAt: z.string().min(1),
  updatedAt: z.string().min(1),
});

export const sessionResponseSchema = z.object({
  accessToken: authTokenSchema,
  expiresAt: z.string().min(1),
  user: userResponseSchema,
  wishlist: listSummaryResponseSchema,
});

export type AuthUser = z.infer<typeof userResponseSchema>;
