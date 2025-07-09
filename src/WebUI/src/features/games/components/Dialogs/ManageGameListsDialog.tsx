import {
    Dialog,
    DialogOverlay,
    DialogCloseButton,
    DialogHeader,
    DialogBody,
    DialogFooter,
    Button,
    Text,
    type DialogProps,
    useDisclosure,
} from "@yamada-ui/react";
import { CustomLinkOverlay } from "~/layouts/CustomLink/CustomLinkOverlay";
// https://images.igdb.com/igdb/image/upload/t_original/co4jni.jpg

type Props = {
    gameId: number;
};
export const ManageGameListsDialog = ({ gameId }: Props) => {
    const { open, onOpen, onClose } = useDisclosure();

    return (
        <Dialog
            open={open}
            onClose={onClose}
            header="孫悟空"
            cancel="わけない"
            onCancel={onClose}
            success="わける"
            onSuccess={onClose}
        >
            <DialogOverlay bg="blackAlpha.300" backdropFilter="blur(10px)" />

            <Text>aaa</Text>
        </Dialog>
    );
};
