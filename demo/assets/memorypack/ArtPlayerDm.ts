import { MemoryPackWriter } from "./MemoryPackWriter.js";
import { MemoryPackReader } from "./MemoryPackReader.js";

export class ArtPlayerDm {
    text: string | null;
    model: number;
    color: string | null;
    time: number;
    border: boolean;
    style: Map<string | null, string | null> | null;

    constructor() {
        this.text = null;
        this.model = 0;
        this.color = null;
        this.time = 0;
        this.border = false;
        this.style = null;

    }

    static serialize(value: ArtPlayerDm | null): Uint8Array {
        const writer = MemoryPackWriter.getSharedInstance();
        this.serializeCore(writer, value);
        return writer.toArray();
    }

    static serializeCore(writer: MemoryPackWriter, value: ArtPlayerDm | null): void {
        if (value == null) {
            writer.writeNullObjectHeader();
            return;
        }

        writer.writeObjectHeader(6);
        writer.writeString(value.text);
        writer.writeInt32(value.model);
        writer.writeString(value.color);
        writer.writeInt32(value.time);
        writer.writeBoolean(value.border);
        writer.writeMap(value.style, (writer, x) => writer.writeString(x), (writer, x) => writer.writeString(x));

    }

    static serializeArray(value: (ArtPlayerDm | null)[] | null): Uint8Array {
        const writer = MemoryPackWriter.getSharedInstance();
        this.serializeArrayCore(writer, value);
        return writer.toArray();
    }

    static serializeArrayCore(writer: MemoryPackWriter, value: (ArtPlayerDm | null)[] | null): void {
        writer.writeArray(value, (writer, x) => ArtPlayerDm.serializeCore(writer, x));
    }

    static deserialize(buffer: ArrayBuffer): ArtPlayerDm | null {
        return this.deserializeCore(new MemoryPackReader(buffer));
    }

    static deserializeCore(reader: MemoryPackReader): ArtPlayerDm | null {
        const [ok, count] = reader.tryReadObjectHeader();
        if (!ok) {
            return null;
        }

        const value = new ArtPlayerDm();
        if (count == 6) {
            value.text = reader.readString();
            value.model = reader.readInt32();
            value.color = reader.readString();
            value.time = reader.readInt32();
            value.border = reader.readBoolean();
            value.style = reader.readMap(reader => reader.readString(), reader => reader.readString());

        }
        else if (count > 6) {
            throw new Error("Current object's property count is larger than type schema, can't deserialize about versioning.");
        }
        else {
            if (count == 0) return value;
            value.text = reader.readString(); if (count == 1) return value;
            value.model = reader.readInt32(); if (count == 2) return value;
            value.color = reader.readString(); if (count == 3) return value;
            value.time = reader.readInt32(); if (count == 4) return value;
            value.border = reader.readBoolean(); if (count == 5) return value;
            value.style = reader.readMap(reader => reader.readString(), reader => reader.readString()); if (count == 6) return value;

        }
        return value;
    }

    static deserializeArray(buffer: ArrayBuffer): (ArtPlayerDm | null)[] | null {
        return this.deserializeArrayCore(new MemoryPackReader(buffer));
    }

    static deserializeArrayCore(reader: MemoryPackReader): (ArtPlayerDm | null)[] | null {
        return reader.readArray(reader => ArtPlayerDm.deserializeCore(reader));
    }
}
