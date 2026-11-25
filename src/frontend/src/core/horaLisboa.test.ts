import { describe, expect, it } from "vitest";
import {
  formatarDataHoraLisboa,
  hojeEmLisboa,
  lisboaParaUtc,
  somarDias,
  utcParaLisboa,
} from "./horaLisboa";

describe("horaLisboa", () => {
  it.each([
    ["2026-11-20", "10:00", "2026-11-20T10:00:00.000Z"], // inverno (UTC+0)
    ["2026-07-14", "09:30", "2026-07-14T08:30:00.000Z"], // verão (UTC+1)
    ["2026-10-25", "00:30", "2026-10-24T23:30:00.000Z"], // antes da mudança da hora
  ])("Lisboa %s %s → UTC %s", (data, hora, utc) => {
    expect(lisboaParaUtc(data, hora)).toBe(utc);
  });

  it("UTC → dia e hora de Lisboa", () => {
    expect(utcParaLisboa("2026-07-14T08:30:00Z")).toEqual({
      data: "2026-07-14",
      hora: "09:30",
    });
    expect(utcParaLisboa("2026-11-20T23:30:00Z")).toEqual({
      data: "2026-11-20",
      hora: "23:30",
    });
  });

  it("dia de hoje em Lisboa e soma de dias", () => {
    expect(hojeEmLisboa(new Date("2026-07-14T23:30:00Z"))).toBe("2026-07-15");
    expect(somarDias("2026-12-30", 6)).toBe("2027-01-05");
  });

  it("formata a data e hora em Lisboa", () => {
    expect(formatarDataHoraLisboa("2026-07-14T08:30:00Z")).toBe(
      "14/07/2026, 09:30",
    );
    expect(formatarDataHoraLisboa(null)).toBe("—");
  });
});
