import moment from 'moment';
import jQuery from 'jquery'

import { SongModel } from "../models/songModels"

export default class Filters {
    static stanzaLongVersion(value: any): string {
        if (!value) return '';
        value = value.toString();
        return value.slice(1);
    }

    static shortDate(date: any): string {
        switch (typeof (date)) {
            case 'string':
            case 'number':
                return date != null ? moment(date.toString()).format('MM/DD/YYYY') : '';
                break;

            default: return '';
        }
    }

    static pluralize(num: number, singleWord: string, pluralWord?: string): string {
        if (num !== 0 && (!num || !singleWord)) {
            return '';
        }

        if (num === 1) {
            return singleWord;
        }

        return pluralWord ? pluralWord : singleWord + 's';
    }

    static spacify(text: string): any {
        return jQuery('<textarea>').html(text.replace(new RegExp(/ /g, "gi"), '&nbsp;')).text();
    }
}

export const filters = new Filters();