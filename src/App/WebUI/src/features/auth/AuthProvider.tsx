import React from "react";
import { useMeProfileGetEndpoint } from "~/api";
import { AuthContext } from "./AuthContext";

//const key = "tanstack.auth.user";
export function AuthProvider({ children }: { children: React.ReactNode }) {
    const { data, isLoading, isError } = useMeProfileGetEndpoint({
        query: { staleTime: 60 * 1000 },
    });

    const user = data?.user;
    const isAuthenticated = !!data && !isLoading && !isError;
    const login = React.useCallback(() => {
        window.location.href = "/signin";
    }, []);

    const logout = React.useCallback(() => {
        window.location.href = "/signout";
    }, []);
    return (
        <AuthContext.Provider value={{ isAuthenticated, user, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
}
