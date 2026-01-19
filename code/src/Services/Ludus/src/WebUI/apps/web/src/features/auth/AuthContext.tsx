import React from "react";
import type { UserDto } from "~/gen";

export interface AuthContext {
    isAuthenticated: boolean;
    login: () => void;
    logout: () => void;
    user: UserDto | undefined;
}
export const AuthContext = React.createContext<AuthContext | null>(null);
