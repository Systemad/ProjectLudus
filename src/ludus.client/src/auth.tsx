import * as React from 'react'
import { useGetApiUserMe, type UserDto } from './gen'

export interface AuthContext {
    isAuthenticated: boolean
    //login: (username: string) => Promise<void>
    //logout: () => Promise<void>
    user: UserDto | null
}

const AuthContext = React.createContext<AuthContext | null>(null);

export function AuthProvider({children}: {children: React.ReactNode}){
    const [user, setUser] = React.useState<UserDto | null>(null);
    const isAuthenticated = !!user;

    const {data, isPending, isError} = useGetApiUserMe();

    React.useEffect(() => {
        
    if(!isPending && !isError && data.data !== undefined){
        setUser(data.data)
    }
    }, [data, isPending, isError])

    return (
        <AuthContext.Provider value={{isAuthenticated, user}}>
            {children}
        </AuthContext.Provider>
    )
}