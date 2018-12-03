const RSAA = "@@redux-api-middleware/RSAA";

export const FETCH_SYSTEM_INFO_STARTED = "FETCH_SYSTEM_INFO_STARTED";
export const FETCH_SYSTEM_INFO_FINISHED = "FETCH_SYSTEM_INFO_FINISHED";

export const fetchSystemInfo = () => ({
  [RSAA]: {
    endpoint: `/api/system/info`,
    method: "GET",
    types: [
      FETCH_SYSTEM_INFO_STARTED,
      FETCH_SYSTEM_INFO_FINISHED,
      "FETCH_SYSTEM_INFO_FAILED"
    ]
  }
});
