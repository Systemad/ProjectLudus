type VideoThumbnailProps = {
    videoId: string;
    onClick?: () => void;
};

export function VideoThumbnail({ videoId, onClick }: VideoThumbnailProps) {
    const thumbnailUrl = `https://i.ytimg.com/vi/${videoId}/maxresdefault.jpg`;

    return (
        <button
            type="button"
            onClick={onClick}
            className="relative h-[96px] w-full rounded-xs overflow-hidden bg-bg-weak opacity-80 hover:opacity-100 transition"
        >
            {/* Thumbnail image */}
            <img
                src={thumbnailUrl}
                alt="Video thumbnail"
                className="absolute left-0 top-0 size-full object-cover"
                style={{ filter: "brightness(0.9)" }}
            />

            {/* Play icon overlay */}
            <div className="absolute inset-0 flex items-center justify-center">
                <div className="rounded-xs bg-transparent-base p-1 backdrop-blur-[8px]">
                    <svg
                        className="text-icon-white-permanent"
                        style={{ width: 24, height: 24 }}
                    >
                        <use href="/sprites/ui.svg#PlaySolid"></use>
                    </svg>
                </div>
            </div>
        </button>
    );
}
