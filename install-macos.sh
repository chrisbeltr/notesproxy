#!/usr/bin/env zsh
set -euo pipefail

if [[ "$(uname -m)" == "arm64" ]]; then
    RID="osx-arm64"
else
    RID="osx-x64"
fi

TARGET_DIR="$HOME/.local/bin"
PUBLISH_DIR="src/NotesProxy.Cli/bin/publish"
SERVER_PUBLISH_DIR="src/NotesProxy.Server/bin/publish"

dotnet publish src/NotesProxy.Cli -r "$RID" -c Release -o "$PUBLISH_DIR"
dotnet publish src/NotesProxy.Server -r "$RID" -c Release -o "$SERVER_PUBLISH_DIR"

mkdir -p "$TARGET_DIR"
cp "$PUBLISH_DIR/notesproxy" "$TARGET_DIR/notesproxy"
chmod +x "$TARGET_DIR/notesproxy"
cp "$TARGET_DIR/notesproxy" "$TARGET_DIR/np"
cp "$SERVER_PUBLISH_DIR/notesproxy-server" "$TARGET_DIR/notesproxy-server"
chmod +x "$TARGET_DIR/notesproxy-server"

if [[ ":$PATH:" != *":$TARGET_DIR:"* ]]; then
    ZSHRC="$HOME/.zshrc"
    if ! grep -qs "$TARGET_DIR" "$ZSHRC" 2>/dev/null; then
        echo "export PATH=\"$TARGET_DIR:\$PATH\"" >> "$ZSHRC"
    fi
    echo "Added '$TARGET_DIR' to PATH in $ZSHRC. Restart your terminal or run: source $ZSHRC"
else
    echo "'$TARGET_DIR' already in PATH."
fi
