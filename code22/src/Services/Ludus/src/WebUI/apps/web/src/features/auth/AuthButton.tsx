import {
    Button,
    Image,
    Avatar,
    Menu,
    MenuButton,
    MenuList,
    MenuItem,
    MenuGroup,
    MenuSeparator,
} from "@yamada-ui/react";
import { useAuth } from "./useAuth";
export const AuthButton = () => {
    const { user, isAuthenticated, login, logout } = useAuth();

    return (
        <>
            {isAuthenticated ? (
                <>
                    <Menu placement={"bottom-end"}>
                        <MenuButton as={Button} p="0" rounded="full" size="lg">
                            <Avatar
                                src={`data:image/png;base64,${user?.userImage?.content}`}
                            />
                        </MenuButton>

                        <MenuList>
                            <MenuGroup>
                                <MenuItem>Profile</MenuItem>
                                <MenuItem>Settings</MenuItem>
                            </MenuGroup>

                            <MenuSeparator />

                            <MenuItem onClick={logout}>Sign out</MenuItem>
                        </MenuList>
                    </Menu>
                </>
            ) : (
                <>
                    <Button padding="0" onClick={login}>
                        <Image src="/steam_login_sits_small.png" />
                    </Button>
                </>
            )}
        </>
    );
};
