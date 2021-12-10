using System.Collections.Generic;
using UnityEngine;

public static class PatternFinder {
    public static List<HitBox> CheckWin(Dictionary<string, HitBox> fields) {
        List<HitBox> match;

        var rows = GameManager.Instance.Rows;
        for (int x = 0; x < rows; x++) {
            for (int y = 0; y < rows; y++) {
                for (int z = 0; z < rows; z++) {
                    // diagonal bottom left, top right
                    match = checkMatch(x, y, z, 1, -1, 0, fields);

                    // diagonal top left, bottom right
                    if (match == null) {
                        match = checkMatch(x, y, z, 1, 1, 0, fields);
                    }

                    // horizontal
                    if (match == null) {
                        match = checkMatch(x, y, z, 1, 0, 0, fields);
                    }

                    // vertical
                    if (match == null) {
                        match = checkMatch(x, y, z, 0, 1, 0, fields);
                    }

                    // vertical Z
                    if (match == null) {
                        match = checkMatch(x, y, z, 0, 0, 1, fields);
                    }

                    // diagonal Z along X lower left to higher right
                    if (match == null) {
                        match = checkMatch(x, y, z, 1, 0, 1, fields);
                    }

                    // diagonal Z along X lower right to higher left
                    if (match == null) {
                        match = checkMatch(x, y, z, -1, 0, 1, fields);
                    }

                    // diagonal Z along Y lower left to higher right
                    if (match == null) {
                        match = checkMatch(x, y, z, 0, 1, 1, fields);
                    }

                    // diagonal Z along Y lower right to higher left
                    if (match == null) {
                        match = checkMatch(x, y, z, 0, -1, 1, fields);
                    }

                    // cross Z lower top left corner, higher bottom right corner
                    if (match == null) {
                        match = checkMatch(x, y, z, 1, 1, 1, fields);
                    }

                    // cross Z lower bottom right corner, higher top left corner
                    if (match == null) {
                        match = checkMatch(x, y, z, -1, -1, 1, fields);
                    }

                    // cross Z lower top right corner, higher bottom left corner
                    if (match == null) {
                        match = checkMatch(x, y, z, -1, 1, 1, fields);
                    }

                    // cross Z lower bottom left corner, higher top right corner
                    if (match == null) {
                        match = checkMatch(x, y, z, 1, -1, 1, fields);
                    }

                    if (match != null) {
                        return match;
                    }
                }
            }
        }

        return null;
    }

    private static List<HitBox> checkMatch(int x, int y, int row, int dX, int dY, int dZ,
        Dictionary<string, HitBox> fields) {
        List<HitBox> hitMatch = new List<HitBox>();
        int type = -1;
        int checkCount = 0;

        var rows = GameManager.Instance.Rows;
        var match = GameManager.Instance.Match;
        while (checkCount < rows && x >= 0 && x < rows &&
               y >= 0 && y < rows) {
            bool found = false;
            var key = $"{x}{y}{row}";
            HitBox marker = fields.ContainsKey(key) ? fields[key] : null;
            if (marker != null && marker.Type != -1) {
                if (type == -1) {
                    type = marker.Type;
                }

                if (marker.Type == type) {
                    hitMatch.Add(marker);
                    found = true;
                }
            }

            if (!found && hitMatch.Count < rows) {
                if (hitMatch.Count >= match) {
                    return hitMatch;
                }

                hitMatch.Clear();
                type = -1;
            }


            x += dX;
            y += dY;
            row += dZ;
            checkCount++;
        }

        return hitMatch.Count >= match ? hitMatch : null;
    }
}