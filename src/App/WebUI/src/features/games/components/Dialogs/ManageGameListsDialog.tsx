import {
    Dialog,
    DialogOverlay,
    Text,
    CheckboxCard,
    CheckboxCardGroup,
    Image,
    Flex,
} from "@yamada-ui/react";

// https://images.igdb.com/igdb/image/upload/t_original/co4jni.jpg

type Props = {
    gameId: string;
    open: boolean;
    //onOpen: () => void;
    onClose: () => void;
};
export const ManageGameListsDialog = ({
    gameId,
    open,
    //onOpen,
    onClose,
}: Props) => {
    return (
        <Dialog
            rounded="xl"
            size="3xl"
            open={open}
            onClose={onClose}
            header={`Game ${gameId}`}
            cancel="Cancel"
            onCancel={onClose}
            success="Save"
            onSuccess={onClose}
        >
            <DialogOverlay bg="blackAlpha.300" backdropFilter="blur(10px)" />

            <CheckboxCardGroup variant={"subtle"} direction={"column"}>
                <CheckboxCard
                    label={
                        <Flex alignItems={"center"} gap="md">
                            <Image
                                objectFit={"cover"}
                                rounded="xl"
                                h="5xs"
                                w="5xs"
                                src="https://images.igdb.com/igdb/image/upload/t_original/co4jni.jpg"
                            ></Image>

                            <Image
                                objectFit={"cover"}
                                ml="-60px"
                                rounded="xl"
                                h="5xs"
                                w="5xs"
                                src="https://images.igdb.com/igdb/image/upload/t_original/co4jni.jpg"
                            ></Image>

                            <Image
                                objectFit={"cover"}
                                ml="-60px"
                                rounded="xl"
                                h="5xs"
                                w="5xs"
                                src="https://images.igdb.com/igdb/image/upload/t_original/co4jni.jpg"
                            ></Image>

                            <Text fontSize={"lg"} fontWeight={"medium"}>
                                Wishlist
                            </Text>
                        </Flex>
                    }
                    value="Wishlist"
                />
                <CheckboxCard
                    label={
                        <Flex alignItems={"center"} gap="md">
                            <Image
                                objectFit={"cover"}
                                rounded="xl"
                                h="5xs"
                                w="5xs"
                                src="https://images.igdb.com/igdb/image/upload/t_original/co4jni.jpg"
                            ></Image>

                            <Image
                                objectFit={"cover"}
                                ml="-60px"
                                rounded="xl"
                                h="5xs"
                                w="5xs"
                                src="https://images.igdb.com/igdb/image/upload/t_original/co4jni.jpg"
                            ></Image>

                            <Image
                                objectFit={"cover"}
                                ml="-60px"
                                rounded="xl"
                                h="5xs"
                                w="5xs"
                                src="https://images.igdb.com/igdb/image/upload/t_original/co4jni.jpg"
                            ></Image>

                            <Text fontSize={"lg"} fontWeight={"medium"}>
                                Favorites
                            </Text>
                        </Flex>
                    }
                    value="Favorites"
                />
            </CheckboxCardGroup>
        </Dialog>
    );
};
