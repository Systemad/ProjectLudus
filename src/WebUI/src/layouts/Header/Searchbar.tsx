import { FileSearchIcon } from "@phosphor-icons/react";
import {
    Box,
    Center,
    dataAttr,
    HStack,
    IconButton,
    Input,
    Kbd,
    Text,
    useDisclosure,
    useUpdateEffect,
    useWindowEvent,
    VStack,
    Modal,
    ModalOverlay,
    ModalCloseButton,
    ModalHeader,
    ModalBody,
    Button,
} from "@yamada-ui/react";
export function HeaderSearchbar() {
    const { open, onClose, onOpen } = useDisclosure();

    return (
        <>
            <HStack
                as="button"
                type="button"
                bg={["bg.subtle/40", "bg.subtle"]}
                color={{ base: "fg.muted", _hover: "fg.emphasized" }}
                cursor="pointer"
                display={{ base: "flex", lg: "none" }}
                h="8"
                minW="fit-content"
                px="sm"
                rounded="l2"
                transitionDuration="moderate"
                transitionProperty="colors"
                onClick={onOpen}
            >
                <Text flex="1" fontSize="sm" lineClamp={1}>
                    Search games
                </Text>

                <HStack gap="xs">
                    <Kbd size="sm" variant="surface" fontSize="sm">
                        ⌘
                    </Kbd>
                    <Kbd size="sm" variant="surface">
                        K
                    </Kbd>
                </HStack>
            </HStack>

            <Button onClick={onOpen} variant="subtle" w="md">
                Search
            </Button>
            <Modal
                size="lg"
                open={open}
                //placement={{ base: "center", sm: "start-center" }}
                withCloseButton={false}
                onClose={onClose}
            >
                <ModalHeader>
                    <Input placeholder="games"></Input>
                </ModalHeader>
                <ModalBody>searh results</ModalBody>
            </Modal>
        </>
    );
}
