import * as React from "react";
import { useMeProfileGetEndpoint, type UserDto } from "./api";

export interface AuthContext {
    isAuthenticated: boolean;
    //login: (username: string) => Promise<void>
    //logout: () => Promise<void>
    user: UserDto | null;
}

const AuthContext = React.createContext<AuthContext | null>(null);

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [user, setUser] = React.useState<UserDto | null>(null);
    const isAuthenticated = !!user;

    const { data, isPending, isError } = useMeProfileGetEndpoint();

    React.useEffect(() => {
        if (!isPending && !isError && data.data.user !== undefined) {
            setUser(data.data.user);
        }
    }, [data, isPending, isError]);

    return (
        <AuthContext.Provider value={{ isAuthenticated, user }}>
            {children}
        </AuthContext.Provider>
    );
}
